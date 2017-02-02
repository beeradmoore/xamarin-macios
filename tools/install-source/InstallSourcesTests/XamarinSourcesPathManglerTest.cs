﻿using System;
using NUnit.Framework;

using InstallSources;

namespace InstallSourcesTests
{
	[TestFixture]
	public class XamarinSourcesPathManglerTest
	{
		string frameworkPath;
		string xamarinSourcePath;
		string installDir;
		XamarinSourcesPathMangler mangler;

		[SetUp]
		public void SetUp()
		{
			frameworkPath = "Xamarin.iOS.framework";
			xamarinSourcePath = "/Users/test/xamarin-macios/src/";
			installDir = "/Library/Frameworks/Xamarin.iOS.framework/";

			mangler = new XamarinSourcesPathMangler {
				FrameworkPath = frameworkPath,
				XamarinSourcePath = xamarinSourcePath,
				InstallDir = installDir
			};
		}

		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/build/ios/native/AVFoundation/AVMutableMetadataItem.g.cs",
			"/Users/test/xamarin-macios/src/build/ios/native/AVFoundation/AVMutableMetadataItem.g.cs")]
		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/build/ios/native/AVFoundation/AVMutableTimedMetadataGroup.g.cs",
		    "/Users/test/xamarin-macios/src/build/ios/native/AVFoundation/AVMutableTimedMetadataGroup.g.cs")]
		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/build/ios/native/CloudKit/CKRecordZoneNotification.g.cs",
		    "/Users/test/xamarin-macios/src/build/ios/native/CloudKit/CKRecordZoneNotification.g.cs")]
		public void TestGetSourcePathGeneratedCode (string path, string expectedPath)
		{
			var result = mangler.GetSourcePath (path);
			Assert.IsTrue (result.Contains ("/build/"), "Path does not contain '/build/'");
			Assert.IsTrue (result.StartsWith(xamarinSourcePath, StringComparison.InvariantCulture), "Path does not start with the XamarinPath '{0}'", xamarinSourcePath);
			Assert.AreEqual (result, expectedPath);
		}

		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/AVFoundation/AVCaptureDeviceInput.cs",
			"/Users/test/xamarin-macios/src/AVFoundation/AVCaptureDeviceInput.cs")]
		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/CoreImage/CIImage.cs",
			"/Users/test/xamarin-macios/src/CoreImage/CIImage.cs")]
		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/CoreData/NSAttributeDescription.cs",
			"/Users/test/xamarin-macios/src/CoreData/NSAttributeDescription.cs")]
		public void TestGetSourcePathManualCode (string path, string expectedPath)
		{
			var result = mangler.GetSourcePath(path);
			Assert.IsFalse (result.Contains ("/build/"), "Path contains '/build/' when it is a manual path.");
			Assert.IsTrue (result.StartsWith (xamarinSourcePath, StringComparison.InvariantCulture), "Path does not start with the XamarinPath '{0}'", xamarinSourcePath);
			Assert.AreEqual (result, expectedPath);
		}

		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/NativeTypes/NMath.cs",
		           "/Users/test/xamarin-macios/src/NativeTypes/NMath.cs")]
		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/build/common/NativeTypes/Drawing.cs",
		           "/Users/test/xamarin-macios/src/build/common/NativeTypes/Drawing.cs")]
		[TestCase ("/Users/test/xamarin-macios/_ios-build/Library/Frameworks/Xamarin.iOS.framework/Versions/git/src/Xamarin.iOS/build/common/NativeTypes/Primitives.cs",
		           "/Users/test/xamarin-macios/src/build/common/NativeTypes/Primitives.cs")]
		public void TestGetSourcePathNativeTypes (string path, string expectedPath)
		{
			var result = mangler.GetSourcePath(path);
			Assert.IsTrue(result.StartsWith(xamarinSourcePath, StringComparison.InvariantCulture), "Path does not start with the XamarinPath '{0}'", xamarinSourcePath);
			Assert.AreEqual (result, expectedPath);
		}

		[TestCase ("/Users/test/xamarin-macios/src/build/ios/native/AVFoundation/AVMutableMetadataItem.g.cs", "/Library/Frameworks/Xamarin.iOS.framework/src/Xamarin.iOS/AVFoundation/AVMutableMetadataItem.g.cs")]
		[TestCase ("/Users/test/xamarin-macios/src/AVFoundation/AVCaptureDeviceInput.cs", "/Library/Frameworks/Xamarin.iOS.framework/src/Xamarin.iOS/AVFoundation/AVCaptureDeviceInput.cs")]
		[TestCase ("/Users/test/xamarin-macios/src/NativeTypes/NMath.cs", "/Library/Frameworks/Xamarin.iOS.framework/src/Xamarin.iOS/NativeTypes/NMath.cs")]
		public void TestGetTargetPath (string src, string expectedTarget)
		{
			var target = mangler.GetTargetPath (src);

			Assert.IsTrue (target.StartsWith (installDir, StringComparison.InvariantCulture), "Does not contain the install dir.");
			Assert.AreEqual (expectedTarget, target, "Target is not the expected one.");
		}
	}
}
