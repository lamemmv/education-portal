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
        new ExtractTextPlugin('style.bundle.css'),
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            "Tether": 'tether',
            Popper: ['popper.js', 'default'],
            // In case you imported plugins individually, you must also require them here:
            Util: "exports-loader?Util!bootstrap/js/dist/util",
            Dropdown: "exports-loader?Dropdown!bootstrap/js/dist/dropdown",
          })
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
                loader: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader']
                }),
            },
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
            {
                test: /\.json$/,
                loader: 'json-loader'
            },

            // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
            {
                enforce: "pre",
                test: /\.js$/,
                loader: "source-map-loader"
            },
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
        extensions: ['.ts', '.tsx', '.js', '.jsx', '.json'],

        alias: {
            'react': 'preact-compat',
            'react-dom': 'preact-compat',
            'create-react-class': 'preact-compat/lib/create-react-class'

        }
    },
    // When importing a module whose path matches one of the following, just
    // assume a corresponding global variable exists and use that instead.
    // This is important because it allows us to avoid bundling all of our
    // dependencies, which allows browsers to cache those libraries between builds.
    externals: {
        //"react": "React",
        //"react-dom": "ReactDOM"
    },
};