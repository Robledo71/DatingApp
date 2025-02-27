namespace API.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly bd)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Validar si la fecha de nacimiento es futura
        if (bd > today)
        {
            return 0;
        }

        var age = today.Year - bd.Year;
        
        // Ajustar si el cumpleaños aún no ha ocurrido este año
        if (bd > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

   
}