using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using FirebaseXamarin.Model;
using Square.Picasso;

namespace FirebaseXamarin.Adapter
{
    public class AdapterMovie : PagerAdapter
    {
        Context context;
        List<ItemMoview> movieList;
        LayoutInflater inflater;

        public AdapterMovie(Context context, List<ItemMoview> movieList)
        {
            this.context = context;
            this.movieList = movieList;
            inflater = LayoutInflater.From(context);
        }

        public override int Count => movieList.Count;

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            ((ViewPager)container).RemoveView((View)@object);
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var itemView = inflater.Inflate(Resource.Layout.view_pager_item, container, false);

            var movieImage = itemView.FindViewById<ImageView>(Resource.Id.imgMovie);
            var movieName = itemView.FindViewById<TextView>(Resource.Id.titleMovie);
            var movieDescription = itemView.FindViewById<TextView>(Resource.Id.txtDescription);
            var btnFav = itemView.FindViewById<ImageView>(Resource.Id.btnFav);

            Picasso.With(context).Load(movieList[position].image).Into(movieImage);
            movieName.Text = movieList[position].name;
            movieDescription.Text = movieList[position].description;

            btnFav.Click += delegate
            {
                Toast.MakeText(context, "Button Clicked", ToastLength.Short).Show();
            };

            itemView.Click += delegate
            {
                Toast.MakeText(context, " " + movieList[position].name, ToastLength.Short).Show();
            };

            container.AddView(itemView);
            return itemView;
        }
    }
}