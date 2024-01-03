var dom = document.getElementById('container');
    var myChart = echarts.init(dom, 'dark', {
      renderer: 'canvas',
      useDirtyRect: false
    });
    var app = {};
    
    var option;

    option = {
  tooltip: {
    trigger: 'axis',
    axisPointer: {
      type: 'cross',
      crossStyle: {
        color: '#999'
      }
    }
  },
  toolbox: {
    feature: {
      dataView: { show: true, readOnly: false },
      magicType: { show: true, type: ['line', 'bar'] },
      restore: { show: true },
      saveAsImage: { show: true }
    }
  },
  legend: {
    data: ['oee', 'Availability', 'Performance', 'Quality', ]
  },
  xAxis: [
    {
      type: 'category',
      data: ['Current Shift', 'Previous Shift'],
      axisPointer: {
        type: 'shadow'
      }
    }
  ],
  yAxis: [
    {
      type: 'value',
      name: 'Availability',
      min: 0,
      max: 100,
      interval: 10,
      axisLabel: {
        formatter: '{value} %'
      }
    },
    {
      type: 'value',
      name: 'oee',
      min: 0,
      max: 100,
      interval: 10,
      axisLabel: {
        formatter: '{value} %'
      }
    }
  ],
  series: [
    {
      name: 'oee',
      type: 'line',
      yAxisIndex: 1,
      tooltip: {
        valueFormatter: function (value) {
          return value + ' %';
        }
      },
      data: [$01, $05]
    },
    {
      name: 'Availability',
      type: 'bar',
      tooltip: {
        valueFormatter: function (value) {
          return value + ' %';
        }
      },
      data: [$02, $06]
    },
    {
      name: 'Performance',
      type: 'bar',
      tooltip: {
        valueFormatter: function (value) {
          return value + ' %';
        }
      },
      data: [$03, $07]
    },
    {
      name: 'Quality',
      type: 'bar',
      tooltip: {
        valueFormatter: function (value) {
          return value + ' %';
        }
      },
      data: [$04, $08]
    }
  ]
};

    if (option && typeof option === 'object') {
      myChart.setOption(option);
    }

    window.addEventListener('resize', myChart.resize);