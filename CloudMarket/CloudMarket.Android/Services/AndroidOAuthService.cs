using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xamarin.Auth.XamarinForms;
using Android.Widget;
using CloudMarket.Droid.Services;
using CloudMarket.Interfaces;
using System.Threading.Tasks;

[assembly: Dependency(typeof(AndroidOAuthService))]
namespace CloudMarket.Droid.Services
{
    public class AndroidOAuthService : Java.Lang.Object
    {
        
    }
}