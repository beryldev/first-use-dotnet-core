/// <binding Clean='clean' />
'use strict';

var gulp = require('gulp'),
  rimraf = require('rimraf'),
  concat = require('gulp-concat'),
  cssmin = require('gulp-cssmin'),
  uglify = require('gulp-uglify');

var paths = {
  webroot: './wwwroot/'
};

//paths.appJs = paths.webroot + 'src/js/**/*.js';
paths.appJs = [
  paths.webroot + 'src/js/app.js',
  paths.webroot + 'src/js/message.service.js',
  paths.webroot + 'src/js/http-interceptor.service.js',
  paths.webroot + 'src/js/doc-list.service.js',
  paths.webroot + 'src/js/new-document.service.js',
  paths.webroot + 'src/js/operation.service.js',
  paths.webroot + 'src/js/app.config.js',
  paths.webroot + 'src/js/controllers/**/*.js'
]

paths.appCss = paths.webroot + 'src/css/*.css';
paths.concatAppJsDest = paths.webroot + 'build/js/app.min.js';
paths.concatAppCssDest = paths.webroot + 'build/css/app.min.css';
paths.concatLibJsDest = paths.webroot + 'build/js/lib.min.js';
paths.concatLibCssDest = paths.webroot + 'build/css/lib.min.css';

paths.libJs = [
    paths.webroot + 'lib/angular/angular.js',
    paths.webroot + 'lib/angular-ui-router/release/angular-ui-router.js',
    paths.webroot + 'lib/angular-animate/angular-animate.js',
    paths.webroot + 'lib/angular-touch/angular-touch.js',
    paths.webroot + 'lib/angular-sanitize/angular-sanitize.js',
    paths.webroot + 'lib/angular-ui-grid/ui-grid.js',
    paths.webroot + 'lib/jquery/dist/jquery.js',
    paths.webroot + 'lib/toastr/toastr.js',
    paths.webroot + 'lib/angular-bootstrap/ui-bootstrap.js',
    paths.webroot + 'lib/angular-bootstrap/ui-bootstrap-tpls.js',
    paths.webroot + 'lib/angular-ui-select/dist/select.js'
];

paths.libCss = [
    paths.webroot + 'lib/bootstrap/dist/css/bootstrap.css',
    paths.webroot + 'lib/bootstrap/dist/css/bootstrap-theme.css',
    paths.webroot + 'lib/angular-ui-grid/ui-grid.css',
    paths.webroot + 'lib/toastr/toastr.css',
    paths.webroot + 'lib/angular-ui-select/dist/select.css',
    paths.webroot + 'lib/font-awesome/css/font-awesome.css'
];

paths.libRes = [
    paths.webroot + 'lib/angular-ui-grid/*.{svg,ttf,woff}'
];

gulp.task('default',['clean:app', 'min:app']);

gulp.task('all',['clean:app', 'clean:lib', 'min:app', 'min:lib']);

//app
gulp.task('clean:app:js', function (cb) {
  rimraf(paths.concatAppJsDest, cb);
});

gulp.task('clean:app:css', function (cb) {
  rimraf(paths.concatAppCssDest, cb);
});

gulp.task('clean:app', ['clean:app:js', 'clean:app:css']);

gulp.task('min:app:js', function () {
  return gulp.src(paths.appJs, { base: '.' })
    .pipe(concat(paths.concatAppJsDest))
    .pipe(uglify())
    .pipe(gulp.dest('.'));
});

gulp.task('min:app:css', function () {
  return gulp.src([paths.appCss])
    .pipe(concat(paths.concatAppCssDest))
    .pipe(cssmin())
    .pipe(gulp.dest('.'));
});

gulp.task('min:app', ['min:app:js', 'min:app:css']);

//lib
gulp.task('clean:lib:js', function (cb) {
  rimraf(paths.concatLibJsDest, cb);
});

gulp.task('clean:lib:css', function (cb) {
  rimraf(paths.concatLibCssDest, cb);
});

gulp.task('clean:lib', ['clean:lib:js', 'clean:lib:css']);

gulp.task('min:lib:js', function () {
  return gulp.src(paths.libJs, { base: '.' })
    .pipe(concat(paths.concatLibJsDest))
    .pipe(uglify())
    .pipe(gulp.dest('.'));
});

gulp.task('min:lib:css', function () {
  return gulp.src(paths.libCss)
    .pipe(concat(paths.concatLibCssDest))
    .pipe(cssmin())
    .pipe(gulp.dest('.'));
});

gulp.task('min:lib', ['min:lib:js', 'min:lib:css', 'copy:lib:res', 'copy:lib:fonts']);

gulp.task('copy:lib:res', function(){
  gulp.src(paths.libRes)
  .pipe(gulp.dest(paths.webroot+'build/css'));
});

gulp.task('copy:lib:fonts', function(){
  gulp.src([paths.webroot + 'lib/font-awesome/fonts/*.{oft,eot,dvg,ttf,woff,woff2}'])
  .pipe(gulp.dest(paths.webroot+'build/fonts'));
});