var dom = document.getElementById('container');
    var myChart = echarts.init(dom, 'dark', {
      renderer: 'canvas',
      useDirtyRect: false
    });
    var app = {};
    
    var option;

    const colors = ['#5470C6', '#91CC75', '#EE6666'];
option = {
  color: colors,
  tooltip: {
    trigger: 'axis',
    axisPointer: {
      type: 'cross'
    }
  },
  grid: {
    right: '20%'
  },
  toolbox: {
    feature: {
      dataView: { show: true, readOnly: false },
      restore: { show: true },
      saveAsImage: { show: true }
    }
  },
  legend: {
    data: ['Energy Consume', 'Energy Cost', 'Carbon']
  },
  xAxis: [
    {
      type: 'category',
      axisTick: {
        alignWithLabel: true
      },
      // prettier-ignore
      data: ['Milwaukee', 'Chicago', 'Singapore', 'Shanghai', 'Bangkok', 'Brussels']
    }
  ],
  yAxis: [
    {
      type: 'value',
      name: 'Energy Consume',
      position: 'right',
      alignTicks: true,
      axisLine: {
        show: true,
        lineStyle: {
          color: colors[0]
        }
      },
      axisLabel: {
        formatter: '{value} MwH'
      }
    },
    {
      type: 'value',
      name: 'Energy Cost',
      position: 'right',
      alignTicks: true,
      offset: 80,
      axisLine: {
        show: true,
        lineStyle: {
          color: colors[1]
        }
      },
      axisLabel: {
        formatter: '{value} M RBM'
      }
    },
    {
      type: 'value',
      name: 'Carbon',
      position: 'left',
      alignTicks: true,
      axisLine: {
        show: true,
        lineStyle: {
          color: colors[2]
        }
      },
      axisLabel: {
        formatter: '{value} kg'
      }
    }
  ],
  series: [
    {
      name: 'Energy Consume',
      type: 'bar',
      data: [$01, $02, $03, $04, $05, $06]
    },
    {
      name: 'Energy Cost',
      type: 'bar',
      yAxisIndex: 1,
      data: [$07, $08, $09, $10, $11, $12]
    },
    {
      name: 'Carbon',
      type: 'line',
      yAxisIndex: 2,
      data: [$13, $14, $15, $16, $17, $18]
    }
  ]
};

    if (option && typeof option === 'object') {
      myChart.setOption(option);
    }

    window.addEventListener('resize', myChart.resize);