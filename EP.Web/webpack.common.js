// We are using node's native package 'path'
// https://nodejs.org/api/path.html
const path = require('path');
const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

// Constant with our paths
const paths = {
    DIST: path.resolve(__dirname, 'dist'),
    SRC: path.resolve(__dirname, 'src'), // source folder path 
    JS: path.resolve(__dirname, 'src/js'),
};

module.exports = {
    entry: {
        app: path.join(paths.JS, 'index.jsx')
    },
    plugins: [
        new CleanWebpackPlugin(['dist']),
        // Tell webpack to use html plugin 
        // index.html is used as a template in which it'll inject bundled app. 
        new HtmlWebpackPlugin({
            title: 'Production',
            template: path.join(paths.SRC, 'index.html'),
        }),
        new ExtractTextPlugin('style.bundle.css')
    ],
    output: {
        filename: '[name].bundle.js',
        path: paths.DIST
    },
    // Loaders configuration 
    // We are telling webpack to use "babel-loader" for .js and .jsx files
    module: {
        rules: [{
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: [
                    'babel-loader',
                ],
            },
            // CSS loader to CSS files 
            // Files will get handled by css loader and then passed to the extract text plugin
            // which will write it to the file we defined above
            {
                test: /\.css$/,
                loader: ExtractTextPlugin.extract({ fallback: 'style-loader', use: ['css-loader'] }),
            },
            // Loader configurations for semantic-ui-less
            // {
            //     // Load .less files from semantic-ui-less module folder
            //     test: /\.less$/i,
            //     include: /[/\\]node_modules[/\\]semantic-ui-less[/\\]/,
            //     use: ExtractTextPlugin.extract({
            //         use: [
            //             // Set importLoaders to 2, because there are two more loaders in the chain (postcss-loader
            //             // and semantic-ui-less-module-loader), which shall be used when loading @import resources
            //             // in CSS files:
            //             {
            //                 loader: 'css-loader',
            //                 options: {
            //                     importLoaders: 2,
            //                     sourceMap: true,
            //                     minimize: true
            //                 }
            //             },
            //             { loader: 'postcss-loader', options: { sourceMap: true } },
            //             {
            //                 loader: 'semantic-ui-less-module-loader',
            //                 options: {
            //                     siteFolder: path.resolve(paths.SRC, 'semantic-ui-theme/site'),
            //                     themeConfigPath: path.resolve(paths.SRC, 'semantic-ui-theme/theme.config'),
            //                 }
            //             }
            //         ]
            //     })
            // },
            // for .less files:
            // {
            //     test: /\.less$/,
            //     use: ExtractTextPlugin.extract({
            //         fallback: 'style-loader',
            //         use: [
            //             { loader: 'css-loader' },
            //             { loader: 'less-loader' }
            //         ]
            //     }),
            //     exclude: [/[\/\\]node_modules[\/\\]semantic-ui-less[\/\\]/]
            // },
            // for semantic-ui-less files:
            // {
            //     test: /\.less$/,
            //     use: ExtractTextPlugin.extract({
            //         fallback: 'style-loader',
            //         use: [
            //             { loader: 'css-loader' },
            //             {
            //                 loader: 'semantic-ui-less-module-loader'
            //             }
            //         ]
            //     }),
            //     include: [/[\/\\]node_modules[\/\\]semantic-ui-less[\/\\]/]
            // },
            // File loader for image assets 
            // We'll add only image extensions, but you can things like svgs, fonts and videos
            {
                test: /\.(png|jpeg|jpg|gif|ttf|eot|svg)$/,
                use: [
                    'file-loader?name=[name].[ext]?[hash]',
                ],
            },
            {
                test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: 'url-loader?limit=10000&mimetype=application/fontwoff'
            },

            // loader for static assets
            // {
            //     test: /\.(png|jpg|jpeg|gif|svg)$/,
            //     use: {
            //         loader: 'url-loader',
            //         options: {
            //             limit: 10240,
            //             absolute: true,
            //             name: 'images/[path][name]-[hash:7].[ext]'
            //         }
            //     },
            //     include: [path.join(__dirname, 'src'), /[\/\\]node_modules[\/\\]semantic-ui-less[\/\\]/]
            // }, {
            //     test: /\.(woff|woff2|ttf|svg|eot)$/,
            //     use: {
            //         loader: 'url-loader',
            //         options: {
            //             limit: 10240,
            //             name: 'fonts/[name]-[hash:7].[ext]'
            //         }
            //     },
            //     include: [path.join(__dirname, 'src'), /[\/\\]node_modules[\/\\]semantic-ui-less[\/\\]/]
            // }
        ],
    },
    // Enable importing JS files without specifying their's extenstion 
    //
    // So we can write:
    // import MyComponent from './my-component';
    //
    // Instead of:
    // import MyComponent from './my-component.jsx';
    resolve: {
        extensions: ['.js', '.jsx'],

        // alias: {
        //     '../../theme.config$': path.join(__dirname, 'semantic-theme/theme.config')
        // }
    },
};