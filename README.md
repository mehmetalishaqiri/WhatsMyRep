### What's My Rep
WhatsMyRep is a simple board displaying developers reputation based on TFS work items.

### Tech Stack
* ASP.NET WEB Api - had to use it because at the time of writing this couldn't find any npm package available to query on premise TFS.
* Node.js
* Express.js
* Jade

#### Quick start
* Download WhatsMyRep Api and published it in IIS.
    * Don't forget to update TfsCollection application settings in web.config
    * Update the developers.json file (App_Data/developers.json). Update the name (same as in TFS) and the email of the developers.It is used mainly for generating Gravatars.
* Download WhatsMyRep [Node.js](https://nodejs.org/) client
    * Don't forget to update api url in routes/index.js file
* Install WhatsMyRep as windows service by executing install-windows-service script.
```
npm run-script install-windows-service
```
* Start WhatsMyRep windows service. WhatMyRep app will be live at http://localhost:3000


### What it looks like ###
![WhatsMyRep Screenshot](https://mshaqiri.files.wordpress.com/2015/03/whatsmyrep.png)

### License
[MIT License](https://github.com/spartanbeg/WhatsMyRep/blob/master/LICENSE)

### Attribution
* [Flat Design UI Components Responsive web template](http://w3layouts.com/flat-design-ui-components) by [W3layouts](http://w3layouts.com)
