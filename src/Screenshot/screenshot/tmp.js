var page = require('webpage').create();
page.open("http://www.google.com", function() {
  page.render("screenshot/615f444431dc4a0daabc4513a21533ee.png");
  phantom.exit();
});