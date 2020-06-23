module.exports = {
  configureWebpack: {
    devtool: "source-map"
  },
  pluginOptions: {
    gitDescribe: {
      variableName: "GIT_DESCRIBE"
    }
  },
  pwa: {
    name: "FCF Verses",
    themeColor: "#22B6D4",
    iconPaths: {
      favicon32: "favicon.ico",
      favicon16: "favicon.ico"
    }
  }
};
