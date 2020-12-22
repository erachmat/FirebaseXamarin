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
using static AndroidX.ViewPager.Widget.ViewPager;

namespace FirebaseXamarin.Transformer
{
    public class DepthPageTransformer : Java.Lang.Object, IPageTransformer
    {
        private static float MIN_SCALE = 0.75f;

        public void TransformPage(View page, float position) {

            int pageWidth = page.Width;
            if (position < -1)
            {
                page.Alpha = 0;
            }
            else if (position <= 0)
            {
                page.Alpha = 1;
                page.TranslationX = 0;
                page.ScaleX = 1;
                page.ScaleY = 1;
            }
            else if (position <= 1)
            {
                page.Alpha = 1 - position;
                page.TranslationX = pageWidth * -position;

                float scaleFactor = MIN_SCALE + (1 - MIN_SCALE) * (1 - Math.Abs(position));
                page.ScaleX = scaleFactor;
                page.ScaleY = scaleFactor;
            }
            else {
                page.Alpha = 0;
            }
        }
    }
}