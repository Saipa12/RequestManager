const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const autoprefixer = require("autoprefixer");
const CopyPlugin = require('copy-webpack-plugin');

module.exports = (env, args) => ([{
    entry: {
        main: "./ts/main.ts"
    },
    devtool: "inline-source-map",
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: "ts-loader",
                exclude: /node_modules/
            },
            {
                test: /\.(s*)css$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: "css-loader",
                        options: {
                            url: false
                        }
                    },
                    {
                        loader: 'postcss-loader',
                        options: {
                            plugins: () => [autoprefixer({ grid: "autoplace" })]
                        }
                    },
                    "sass-loader"
                ]
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2)$/,
                use: "url-loader?name=[name].[ext]"
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].min.css"
        })
    ],
    resolve: {
        extensions: [".tsx", ".ts", ".js"]
    },
    output: {
        library: "blacksun",
        libraryTarget: "var",
        filename: "[name].min.js",
        path: path.resolve(__dirname, "../../BlackSun.Server/wwwroot")
    },
}]);