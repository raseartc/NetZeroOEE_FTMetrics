var dom = document.getElementById('container');
    var myChart = echarts.init(dom, 'dark', {
      renderer: 'canvas',
      useDirtyRect: false
    });
    var app = {};
    
    var option;

    option = {
  title: {
    text: ''
  },
  legend: {
    data: ['Current Shift', 'Previous Shift']
  },
  radar: {
    shape: 'circle',
    indicator: [
      { name: 'oee', max: 100 },
      { name: 'Availability', max: 100 },
      { name: 'Performance', max: 100 },
      { name: 'Quality', max: 100 },
      //{ name: 'Development', max: 52000 },
      //{ name: 'Marketing', max: 25000 }
    ]
  },
  series: [
    {
      name: 'Current vs Previous',
      type: 'radar',
      data: [
        {
          value: [$01, $02, $03, $04],
          name: 'Current Shift'
        },
        {
          value: [$05, $06, $07, $08],
          name: 'Previous Shift'
        }
      ]
    }
  ]
};

    if (option && typeof option === 'object') {
      myChart.setOption(option);
    }

    window.addEventListener('resize', myChart.resize);