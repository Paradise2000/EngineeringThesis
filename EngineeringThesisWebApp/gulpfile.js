const { src, dest, watch, series } = require('gulp')
const sass = require('gulp-sass')(require('sass'))
const replace = require('gulp-replace');
const exec = require('gulp-exec');

function buildStyles() {
    return src('src/scss/*.scss')
        .pipe(sass())
        .pipe(dest('css'))
}

function replaceApiBaseUrl() {
    return src('services/functionService.js')
        .pipe(replace(/export const API_BASE_URL = '.*?';/, `export const API_BASE_URL = 'https://engineeringthesisletstravel.azurewebsites.net';`))
        .pipe(dest('services'));
}

function installDependencies() {
    return src('.')
        .pipe(exec('npm install'))
        .pipe(exec.reporter());
}

function watchTask() {
    watch(['src/scss/*.scss'], buildStyles)
}

exports.default = series(buildStyles, watchTask)
exports.deployApp = series(installDependencies, buildStyles, replaceApiBaseUrl)