const { src, dest, watch, series } = require('gulp')
const sass = require('gulp-sass')(require('sass'))
const replace = require('gulp-replace');

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

function watchTask() {
    watch(['src/scss/*.scss'], buildStyles)
}

exports.default = series(buildStyles, watchTask)
exports.deployApp = series(buildStyles, replaceApiBaseUrl)