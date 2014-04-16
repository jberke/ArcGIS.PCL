﻿using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace ArcGIS.PCL.iOS
{
	public partial class ArcGIS_PCL_iOSViewController : UIViewController
	{
		public ArcGIS_PCL_iOSViewController () : base ("ArcGIS_PCL_iOSViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
            var gateway = new ArcGISGateway(new JsonDotNetSerializer());
            var site = gateway.DescribeSite().Result;

            
		}
	}
}
