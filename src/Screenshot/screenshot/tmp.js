var page = require('webpage').create();
page.open("http://www.google.com", function() {
  page.render("screenshot/8bec8212ef40436d8fe51f3aad350a77.png");
  phantom.exit();
});