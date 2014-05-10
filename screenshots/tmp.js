var page = require('webpage').create();
page.open("https://www.deerso.com", function() {
  page.render("deerso.png");
  phantom.exit();
});