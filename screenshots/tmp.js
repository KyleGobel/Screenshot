var page = require('webpage').create();
page.open("http://www.google.com", function() {
  page.render("screenshots/db58a63eaffe482b994f8050ecf29a68.png");
  phantom.exit();
});