var page = require('webpage').create();
page.open("http://www.pluralsight.com", function() {
  page.render("screenshot/3b8d5746369a40d7a312c89b1ee829e6.png");
  phantom.exit();
});