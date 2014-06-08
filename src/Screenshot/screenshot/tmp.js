var page = require('webpage').create();
page.open("http://www.google.com", function() {
  page.render("screenshot/d1bf52a2f18b4c288ff6d6a6ed699639.png");
  phantom.exit();
});