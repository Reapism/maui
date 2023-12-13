﻿using NUnit.Framework;
using UITest.Appium;
using UITest.Core;
  
namespace Microsoft.Maui.AppiumTests
{
	public abstract class CollectionViewUITests : UITest
	{
		const string CollectionViewGallery = "CollectionView Gallery";

		public CollectionViewUITests(TestDevice device)
			: base(device)
		{
		}

		protected override void FixtureSetup()
		{
			base.FixtureSetup();
			App.NavigateToGallery(CollectionViewGallery);
		}

		protected override void FixtureTeardown()
		{
			base.FixtureTeardown();
			this.Back();
		}

		[TearDown]
		public void CollectionViewUITestTearDown()
		{
			this.Back();
		}

		internal void VisitInitialGallery(string collectionTestName)
		{
			var galleryName = $"{collectionTestName} Galleries";
			var regexGalleryName = System.Text.RegularExpressions.Regex.Replace(galleryName, " |\\(|\\)", string.Empty);

			App.WaitForElement(regexGalleryName);
			App.Click(regexGalleryName);
		}

		internal void VisitSubGallery(string galleryName)
		{
			App.WaitForElement(galleryName);
			App.Click(galleryName);
		}
	}
}