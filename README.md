# CicekSepeti Clone

In this project i tried to replicate ciceksepeti.com


## Installation

To make the project work on computer please install the database that i provided with files. To do this 

Sql Management Studio -> Right click database folder and Import Data-Tier App -> Next -> Select the file that i provided.

After that you have to change connection string of course.

To change connection string, simply open the 

```bash
ConnectionString.cs
```
file that you can find in the root of the project. Simply change connection string to your connection string.

```bash
namespace CicekSepetiCloneDotNet
{
    public static class ConnectionStrings
    {
        public static string DefaultConnection =>
            "Your_Connection_String";
    }
}
```

## Properties

I've used .Net Core to create the project. Theres 3 user roles: user, seller and admin.

You can order, filter and leave a comment (that you bought) on products. 

Every role, except user, have it's own control panel. Admins and Sellers are still users. 

### Admin Role

* Can acces and create / edit / delete all users, comments, categories and products.
* Can assign the seller or admin role to a user.

### Seller Role
* Can create and edit its own products and categories. 
* Will receive a message (that created with trigger) when there's a new order.
* Can mark messages as read.
* Can accept orders.

### User Role
* Can add items to cart.
* Can order the items that.
* Users can leave reviews for the products they have purchased

### Login - Logout
* You can register and reset your password.
* After logging in, you can logout by pressing the Logout button which appears after you login.

### Cart
* You can update the quantity of the product.
* Products can be deleted .
* Total price will update itself.

### My Orders Page

* You can see your recent and old orders in this page.
* You can leave reviews for the products listed in the "Previous Orders" table.
### Product Preview Page
* You'll be redirected to this page when you click one of the products in Index.
* It contains features of that specific product.
* There's comment section on the bottom of the page along with comment counter.
### Payment Page
* Your information will be filled automatically because you have given your informations at the registration page
* There is a table containing the order summary.

## Sql
* There's 3 functinos, triggers, 9 stored procedures and 1 view.
* Triggers are used for archiving deleted products, creating messages for orders and updating the stocks if there's any order.
* Functions are used for to get total quantity of products, to get total number of sellers, to get to numbers of items that sold.
* A view to show product information with category, seller name by combining tbl_categories and tbl_users with tbl_products.
