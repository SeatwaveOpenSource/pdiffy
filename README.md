
# P Diffy [![Build status](https://ci.appveyor.com/api/projects/status/ssokmy3420v2rrj4?svg=true&style=flat-square)](https://ci.appveyor.com/project/masduo/pdiffy)

## A perceptual difference tool for image and text comparisons

![alt text](https://github.com/SeatwaveOpenSource/pdiffy/blob/master/pdiffy_logo.PNG "P Diffy")
***

## Running the web app
1. Clone this repo
2. Restore Nuget Packages
3. Run this using Visual Studio (2013 or later) or host it somewhere **(run it as an admin)**
4. Add the environment variables *pdiffyImageStorePath* and *pdiffyDataStorePath* and set the values to wherever you want to store the images and data P Diffy requires

## Node & Gulp considerations
P Diffy uses node and gulp in order to compile stylesheets and generate javascripts for each feature. The generated files are already source controlled under root /Styles and /Scripts folders and ready to use. But in case any of the Feature/x/ css or js files are editted running gulp will regenrate them. Steps to follow:

1. Install <a href="http://nodejs.org/" target="_blank">Node.js</a>
2. Open command prompt and navigate to /Infrastructure/FrontEndConfiguration
3. To get all packages required in packages.json run "npm install"
4. To get gulp going based on instructions on gulfile.js run "gulp dev"

This should (as instructed in gulpfile.js)

- Generate compiled css files into /Styles/ folder
- Generate javascript files into /Scripts/

## Using P Diffy

P Diffy performs comparisons via image and/or via text. The results are stored in separate files and reported via different uris.

### Image Comparison **/api/imagecomparisons**
There are two ways to use P Diffy to generate difference images using P Diffy.

#### 1. Host the images yourself.
Endpoint: **/api/imagecomparisons?name=key&imageUrl=yourimageurl**

* The first time you hit this endpoint with an image url P Diffy will create the base for future comparisons.
* The second time you hit this endpoint with an image url P Diffy will compare the image at the given url with the image at the original url and generate a difference image if necessary.

#### 2. Let P Diffy host the images.
Endppoint: **/api/imagecomparisons?name=key**

This endpoint requires the image that will be compared to be streamed to it. (see example below)

```csharp
var request = HttpWebRequest.Create("//yourhosting.something/api/imagecomparisons?name=key");
request.Method = "POST";
using (var stream = request.GetRequestStream())
using (var docFile = File.OpenRead(@"C:\Images\unicorn.png"))
	docFile.CopyTo(stream);

var response = request.GetResponse();
```

* The first time you hit this endpoint with a streamed image P Diffy will store the image and create the base for future comparisons.
* The second time you hit this endpoint with streamed image P Diffy will save the newly streamed image and compare it to the orignal image, creating a difference image if necessary.

### Text Comparison **/api/textcomparisons**
This is a more simplified version of the tool, as there is no need to host the images. 

Endpoint: **/api/textcomparisons?name=key**

Likewise image comparison, P Diffy will store a base line against the passed in key (?name) on first hit, and will perform text comparisons and store the results on subsequent calls to the api using the same key. (see example below)

```csharp
var request = HttpWebRequest.Create("//yourhosting.something/api/textcomparisons?name=key");
request.Method = "POST";
using (var stream = request.GetRequestStream())
using (var ms = new MemoryStream(Encoding.Unicode.GetBytes("some text to compare")))
	ms.CopyTo(stream);

var response = request.GetResponse();
```

## Comparison Results

* For image comparisons: //yourhosting.something/imagedifferences
* For text comparisons: //yourhosting.something/textdifferences

## Urls

* **http://yourhosting.something**
* **http://yourhosting.something/api/imagecomparisons?name=key&imageUrl=yourimageurl**
* **http://yourhosting.something/api/imagecomparisons?name=key**
* **http://yourhosting.something/api/textcomparisons?name=key**
