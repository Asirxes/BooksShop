# BooksShop

Online book store application created for using .NET MVC and Razor Pages. Provides database service entity framework in conjunction with MSQ Server. The visual side is supported by bootstrap. The project implements the concept of "areas", "identity", sending e-mails using mailkit and mimekit as well as the SendGrid service. Thanks to the "Meta for Developers" service for applications it is possible registering and logging in with a facebook account. The payment operation is carried out using the Stripe service. Some components were provided by third parties such as data table.

Thanks to the azure service, the application is available at https://asirxesbookstore.azurewebsites.net/

To use the project, you need to create a fake account or register via facebook. You will be logged in automatically. Then you can add books to your shopping cart. Then you can buy them and make fake payment using stripe (input these for stripe: test@gmail.com,4242424242424242,123,08/26,testtest).

Thanks to the panel in the upper right corner by clicking hello {your name} button you can edit your profile.

To take full advantage of the application and see its functionality, I recommend loggin as an Admin using "Admin@wp.pl" and "Admin123." Then you can manage covers, categories, orders, books, company accounts and their orders.

Company users can buy books with delayed payments, they can pay them with admin usage or later on.
