About The App:

This is a simple POC on Cookies using Asp.net MVC framework. After Cloning the App you need create a SQL DB (Name: [LogInDB]) and run the below SQL Script to create a simple table. You can enter any username and Password into the table directly. 

Note: In the App I've used SHA1.cs class to encrypt the password. You can avoid using the SHA1.cs class. If you are using the class to encrypt the password then your DB should have the encrypted value of the password.

Eg. test = a94a8fe5ccb19ba61c4c0873d391e987982fbbd3;

You can Host the App in IIS so that you can test the cookie behaviour properly (E.g closing the browser and re-opening again and then try to hit the Uri without logging in).

SQL Script:

USE [LogInDB]
GO

/****** Object:  Table [dbo].[System_Users]    Script Date: 11-11-2017 18:11:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[System_Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[RegDate] [datetime] NOT NULL DEFAULT (getdate()),
	[Email] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO



***************************************************************************************************************************************


About Cookies:

Persistent Cookie is a Cookie with an Expiry time. If you dont set and Expiry time to your Cookie it becomes non-persistent.

Persistent Cookie only gets removed with its own expiration time. It remains even if the browser closes or the Auth-ticket Expires (Web.config -- time out), but once Auth-ticket is expired, Persistent Auth-Cookie alone can't keep you logged-in. The same way if Persistent Auth-Cookie expires before your Auth-ticket, you will be logged-out. So we need to keep these two in sync. 

E.g . Suppose you set TimeOut of your Auth Token to 1 hour and kept the same TimeOut for your Auth Cookie, with-in this time frame you will be logged-in even if you close the browser. (given that your browser's "Keep local data only until you quit your browser" is not checked )

You need not keep all the other Persistent Cookies in sync with the Auth-ticket but only the Auth-Cookie.


Non-Persistent Cookie get removed if the browser is closed or the Auth-ticket expires (Web.config -- time out).

So if you close your browser even before Auth-ticket has expired you will lose the Cookie and have to log-in again or If your Auth-Ticket Expires even when the browser is open, Cookie will be removed.


                                     ************************* Note *************************

If you keep -- slidingExpiration="True", Auth-ticket's Time-Out gets extended with every server interaction and it will Extend the Auth-cookie's TimeOut as-well. Below is the code block which creates the Auth Tocken, encrypts it, attaches it to the Auth Cookie and then sets the Cookie Time-out.

 
FormsAuthenticationTicket authTicket
                        = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now, 
                        DateTime.Now.Add(FormsAuthentication.Timeout), user.RememberMe, "your custom data");

                    string encryptedAuthTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedAuthTicket);

                    if (user.RememberMe)
                        authCookie.Expires = authTicket.Expiration;

                    authCookie.Path = FormsAuthentication.FormsCookiePath;
                    Response.Cookies.Add(authCookie);

slidingExpiration="True", will not Extend Expiry of any othe Cookies apart from Auth-Cookie.


                                          *************** Browser Settings ****************

In Chromes content setting section when we check the option "Keep local data only until you quit your browser" , Chrome will delete all the cookies once you close your browser irrespective of the cookie type (persistent/non-persistent), however if you unchecked that then persistent cookies will be stored in your local even when you close your browser for your later use and will expire according to the server code or Expiration time written in the cookie.

Eg. You log-in to GitHub. GitHub drops a cookie into your local floder. All the time you were logged-in to GitHub account you used that cookie for identifying yourself to the GitHub server, now you closed the GitHub tab and opened Facebook, Facebook server drops a fb cookie into your local folder. Now your local has both fb cookie and GitHub cookie. You need not log-in again in-order to use any of these sites even if you fully close you browser, unless your locally stored cookies expire according to the cookie expiration time provided by the server.

But if your "Keep local data only until you quit your browser" is checked, you will lose all the locally stored cookies once when you close the browser and you are going to have to log-in back again in-order for the fb and GitHub server to identify you.


Chrome stores its cookies in the location given below:

C:\Users\Admin\AppData\Local\Google\Chrome\User Data\Default

To read it you can use online sql-lite:

https://sqliteonline.com/

