using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using VideoDownloader.Models;
using Microsoft.Extensions.Http;

public class LoginModel : PageModel
{
    private readonly MyDbContext _context;

    public LoginModel(MyDbContext context)
    {
        _context = context;
        Login = new Login(); 
    }

    [BindProperty]
    public Login Login { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        
        var email = Login.Email;

        
        var login = new Login { Email = email, Created = DateTime.Now };
        _context.Logins.Add(login);
        await _context.SaveChangesAsync();

        
        var loginId = login.Id;

        
        Response.Cookies.Append("EmailID", loginId.ToString());
        Response.Cookies.Append("Email", email);

        
        return RedirectToPage("/Index");
    }


}
