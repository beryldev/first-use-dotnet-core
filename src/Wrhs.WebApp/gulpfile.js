/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  uglify = require("gulp-uglify");

var paths = {
  webroot: "./wwwroot/"
};

paths.appJs = paths.webroot + "src/js/**/*.js";
paths.appCss = paths.webroot + "src/css/**/*.css";
paths.concatAppJsDest = paths.webroot + "build/js/app.min.js";
paths.concatAppCssDest = paths.webroot + "build/css/app.min.css";
paths.concatLibJsDest = paths.webroot + "build/js/lib.min.js";
paths.concatLibCssDest = paths.webroot + "build/css/lib.min.css";

paths.libJs = [
    paths.webroot + "lib/angular/angular.js",
    paths.webroot + "lib/angular-ui-router/release/angular-ui-router.js",
    paths.webroot + "lib/angular-animate/angular-animate.js",
    paths.webroot + "lib/angular-touch/angular-touch.js"
];

paths.libCss = [
    paths.webroot + "lib/bootstrap/dist/css/bootstrap.css"
];

gulp.task("default",["clean:app", "min:app"]);

gulp.task("all",["clean:app", "clean:lib", "min:app", "min:lib"]);

//app
gulp.task("clean:app:js", function (cb) {
  rimraf(paths.concatAppJsDest, cb);
});

gulp.task("clean:app:css", function (cb) {
  rimraf(paths.concatAppCssDest, cb);
});

gulp.task("clean:app", ["clean:app:js", "clean:app:css"]);

gulp.task("min:app:js", function () {
  return gulp.src([paths.appJs], { base: "." })
    .pipe(concat(paths.concatAppJsDest))
    .pipe(uglify())
    .pipe(gulp.dest("."));
});

gulp.task("min:app:css", function () {
  return gulp.src([paths.appCss])
    .pipe(concat(paths.concatAppCssDest))
    .pipe(cssmin())
    .pipe(gulp.dest("."));
});

gulp.task("min:app", ["min:app:js", "min:app:css"]);

//lib
gulp.task("clean:lib:js", function (cb) {
  rimraf(paths.concatLibJsDest, cb);
});

gulp.task("clean:lib:css", function (cb) {
  rimraf(paths.concatLibCssDest, cb);
});

gulp.task("clean:lib", ["clean:lib:js", "clean:lib:css"]);

gulp.task("min:lib:js", function () {
  return gulp.src(paths.libJs, { base: "." })
    .pipe(concat(paths.concatLibJsDest))
    .pipe(uglify())
    .pipe(gulp.dest("."));
});

gulp.task("min:lib:css", function () {
  return gulp.src(paths.libCss)
    .pipe(concat(paths.concatLibCssDest))
    .pipe(cssmin())
    .pipe(gulp.dest("."));
});

gulp.task("min:lib", ["min:lib:js", "min:lib:css"]);