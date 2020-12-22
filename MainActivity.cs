using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using FirebaseXamarin.Adapter;
using Firebase.Database;
using FirebaseXamarin.Interface;
using FirebaseXamarin.Model;
using System.Collections.Generic;
using FirebaseXamarin.Transformer;
using System;
using System.Linq;

namespace FirebaseXamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IValueEventListener, IFirebaseLoadDone
    {

        ViewPager viewPager;
        AdapterMovie adapterMovie;

        DatabaseReference movies;

        IFirebaseLoadDone firebaseLoadDone;

        List<ItemMoview> moviewList = new List<ItemMoview>();

        protected override void OnCreate(Bundle savedInstanceState)
        {  
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            firebaseLoadDone = this;

            viewPager = FindViewById<ViewPager>(Resource.Id.vpMovie);
            viewPager.SetPageTransformer(true, new DepthPageTransformer());

            movies = FirebaseDatabase.Instance.GetReference("Movies");

            LoadMovie();
        }

        private void LoadMovie()
        {
            movies.AddListenerForSingleValueEvent(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnCancelled(DatabaseError error)
        {
            firebaseLoadDone.OnFirebaseLoadFailed(error.Message);
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            moviewList.Clear();

            foreach (DataSnapshot movieSnapShot in snapshot.Children.ToEnumerable()) {

                ItemMoview itemMoview = new ItemMoview();

                itemMoview.name = movieSnapShot.Child("name").GetValue(true).ToString();
                itemMoview.description = movieSnapShot.Child("description").GetValue(true).ToString();
                itemMoview.image = movieSnapShot.Child("image").GetValue(true).ToString();

                moviewList.Add(itemMoview);
            }

            firebaseLoadDone.OnFirebaseLoadSuccess(moviewList);
        }

        public void OnFirebaseLoadSuccess(List<ItemMoview> movieList)
        {
            adapterMovie = new AdapterMovie(this, movieList);
            viewPager.Adapter = adapterMovie;
        }

        public void OnFirebaseLoadFailed(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }
    }
}