import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { setPaginationHeaders, setPaginationResponse } from './paginationHelper';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/pagination';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';Add commentMore actions
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubsUrl;
  private http = inject(HttpClient);
  private hubConnection?: HubConnection;
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);
  messageThread = signal<Message[]>([]);

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + "messages?user=" + otherUsername, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on("ReceiveMessageThread", messages => {
      this.messageThread.set(messages);
    });
  }

  stopHbuConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append("Container", container);

    return this.http.get<Message[]>(this.baseUrl + "messages",
      { observe: "response", params }).subscribe({
        next: response => setPaginationResponse(response, this.paginatedResult)
      });
  }
}