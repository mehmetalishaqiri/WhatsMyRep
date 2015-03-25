var express = require('express');
var router = express.Router();
var http = require("http");

/* GET home page. */
router.get('/', function (req, res) {
    var opts = {
        host: "api.whatsmyrepo.crea-ko.com",
        port: 80,
        path: "/api/reputation",
        method: "GET",
        headers : {
            "User-Agent": "What's My Rep"
        }
    }
    
    http.get(opts, function (response) {
        
        var buffer = "", 
            data;
        
        response.on("data", function (chunk) {
            buffer += chunk;
        });
        
        response.on("end", function (err) {
            
            data = JSON.parse(buffer);
            res.render('index', {
                title: "What's My Reputation?", 
                developers : data
            });
        
        });
    }).on('error', function (error) {
        res.render('index', {
            title: "What's My Reputation?", 
            developers : []
        });
    });
});

module.exports = router;