/// <binding BeforeBuild='clean' AfterBuild='copy' />
module.exports = function (grunt) {
    //Configuration setup
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        copy: {
            domain: {
                expand: true,
                cwd: 'DexCMS.Tickets/bin/Release/',
                src: ['DexCMS.Tickets.dll'],
                dest: 'dist/'
            },
            mvc: {
                expand: true,
                cwd: 'DexCMS.Tickets.Mvc/bin/Release/',
                src: ['DexCMS.Tickets.Mvc.dll'],
                dest: 'dist/'
            },
            webapi: {
                expand: true,
                cwd: 'DexCMS.Tickets.WebApi/bin/Release/',
                src: ['DexCMS.Tickets.WebApi.dll'],
                dest: 'dist/'
            }
        },
        clean: {
            build: ["dist"]
        }
    });

    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-clean');

    grunt.registerTask('default', ['clean', 'copy']);
};
