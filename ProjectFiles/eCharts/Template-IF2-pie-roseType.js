var dom = document.getElementById('container');
    var myChart = echarts.init(dom, 'dark', {
      renderer: 'canvas',
      useDirtyRect: false
    });
    var app = {};
    
    var option;

    option = {
  legend: {
    top: 'bottom'
  },
  toolbox: {
    show: true,
    feature: {
      mark: { show: true },
      dataView: { show: true, readOnly: false },
      restore: { show: true },
      saveAsImage: { show: true }
    }
  },
  series: [
    {
      name: 'Nightingale Chart',
      type: 'pie',
      radius: [50, 250],
      center: ['50%', '50%'],
      roseType: 'area',
      itemStyle: {
        borderRadius: 8
      },
      data: [
        { value: $01, name: 'Current-oee' },
        { value: $02, name: 'Current-Availability' },
        { value: $03, name: 'Current-Performance' },
        { value: $04, name: 'Current-Quality' },
        { value: $05, name: 'Previous-oee' },
        { value: $06, name: 'Previous-Availability' },
        { value: $07, name: 'Previous-Performance' },
        { value: $08, name: 'Previous-Quality' }
      ]
    }
  ]
};

    if (option && typeof option === 'object') {
      myChart.setOption(option);
    }

    window.addEventListener('resize', myChart.resize);