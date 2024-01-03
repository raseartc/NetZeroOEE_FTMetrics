<!--
	THIS EXAMPLE WAS DOWNLOADED FROM https://echarts.apache.org/examples/en/editor.html?c=dataset-simple0&theme=dark&code=PYBwLglsB2AEC8sDeAoWsA2BTA5l6AJgFzIC-ANGrGMMBpCCUhVQQIZhsDOWYTV6LsACuAJwDGWEgG0B6WNIDkIUcALDxYReViKA8gFED23QEEAbmwgY2AI2sQwATxOKACllEAzYKIC2bNCSrgCKwmwYji4AupTy8kpuoljmUMI8sADKABYQXlo6ACwAzAB0xToAHACspZU6AJxlAOw6AIwADKXVsXLoSgDCYsnQYFm5-SaVZW06zWWFOtW1s8u98egA9JuD2VhYGQPA4sBsUwBspYuw57UATFV33esb20oA6hHQwmMAQqoAd2gECwJmaT2u1TKDR0xQapTa0TkSPQLHQAA9TOiIFwmNQnCApLpxBxcL4XLA0bAnFicUwqdtYAARLDiGzJWA8cyeCKwWxsUSczwgrg6LBscTZWAA6wYPlYWABECEghURk0WBsWAnDDCPxwYBeWDsTg8MClIRiSR8pzGrBeNjCeilKg8UQimRIfGEkiKfmiRSUnRe5w-3T-wMUZDeol-gWR4Mx30RylI0gAbiAA
	⚠ Please be aware that this chart is not an official demo of Apache ECharts but is made by user-generated code.
-->
<!DOCTYPE html>
<html lang="en" style="height: 100%">
<head>
  <meta charset="utf-8">
</head>
<body style="height: 100%; margin: 0">
  <div id="container" style="height: 100%"></div>

  
  <script type="text/javascript" src="https://fastly.jsdelivr.net/npm/echarts@5.4.2/dist/echarts.min.js"></script>
  <!-- Uncomment this line if you want to dataTool extension
  <script type="text/javascript" src="https://fastly.jsdelivr.net/npm/echarts@5.4.2/dist/extension/dataTool.min.js"></script>
  -->
  <!-- Uncomment this line if you want to use gl extension
  <script type="text/javascript" src="https://fastly.jsdelivr.net/npm/echarts-gl@2/dist/echarts-gl.min.js"></script>
  -->
  <!-- Uncomment this line if you want to echarts-stat extension
  <script type="text/javascript" src="https://fastly.jsdelivr.net/npm/echarts-stat@latest/dist/ecStat.min.js"></script>
  -->
  <!-- Uncomment this line if you want to use map
  <script type="text/javascript" src="https://fastly.jsdelivr.net/npm/echarts@4.9.0/map/js/china.js"></script>
  <script type="text/javascript" src="https://fastly.jsdelivr.net/npm/echarts@4.9.0/map/js/world.js"></script>
  -->
  <!-- Uncomment these two lines if you want to use bmap extension
  <script type="text/javascript" src="https://api.map.baidu.com/api?v=3.0&ak=YOUR_API_KEY"></script>
  <script type="text/javascript" src="https://fastly.jsdelivr.net/npm/echarts@5.4.2/dist/extension/bmap.min.js"></script>
  -->

  <script type="text/javascript">
    var dom = document.getElementById('container');
    var myChart = echarts.init(dom, 'dark', {
      renderer: 'canvas',
      useDirtyRect: false
    });
    var app = {};
    
    var option;

    option = {
  legend: {},
  tooltip: {},
  dataset: {
    source: [
      ['product', 'OEE', 'Availability', 'Performance', 'Quality'],
      ['Current Shift', $01, $02, $03, $04],
      ['Previouse Shift', $05, $06, $07,$08]
      //['Cheese Cocoa', 86.4, 65.2, 82.5],
      //['Walnut Brownie', 72.4, 53.9, 39.1]
    ]
  },
  xAxis: { type: 'category' },
  yAxis: {},
  // Declare several bar series, each will be mapped
  // to a column of dataset.source by default.
  series: [{ type: 'bar' }, { type: 'bar' }, { type: 'bar' }, { type: 'bar' }]
};

    if (option && typeof option === 'object') {
      myChart.setOption(option);
    }

    window.addEventListener('resize', myChart.resize);
  </script>
</body>
</html>