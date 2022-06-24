var myMap;

ymaps.ready(function () {
    myMap = new ymaps.Map('map', {
           center: [48.045, 37.92],
           zoom: 12
       }, {
           searchControlProvider: 'yandex#search'
       }),

       // Создаём макет содержимого.
       MyIconContentLayout = ymaps.templateLayoutFactory.createClass(
           '<div style="color: #FFFFFF; font-weight: bold;">$[properties.iconContent]</div>'
       ),

       
    
       myPlacemarkWithContent = new ymaps.Placemark([48.039721, 37.867762], {
           hintContent: 'улица Малиновского, 59.Универмаг.Магазин бытовой техники Технобум',
       }, {
           // Опции.
           // Необходимо указать данный тип макета.
           iconLayout: 'default#imageWithContent',
           
           // Размеры метки.
           iconImageSize: [48, 48],
           // Смещение левого верхнего угла иконки относительно
           // её "ножки" (точки привязки).
           iconImageOffset: [-24, -24],
           // Смещение слоя с содержимым относительно слоя с картинкой.
           iconContentOffset: [15, 15],
           // Макет содержимого.
           iconContentLayout: MyIconContentLayout
       });
       myPlacemarkWithContent1 = new ymaps.Placemark([48.043719, 37.974900], {
        hintContent: '.Универмаг.Магазин бытовой техники Технобум',
    }, {
        // Опции.
        // Необходимо указать данный тип макета.
        iconLayout: 'default#imageWithContent',
        
        // Размеры метки.
        iconImageSize: [48, 48],
        // Смещение левого верхнего угла иконки относительно
        // её "ножки" (точки привязки).
        iconImageOffset: [-24, -24],
        // Смещение слоя с содержимым относительно слоя с картинкой.
        iconContentOffset: [15, 15],
        // Макет содержимого.
        iconContentLayout: MyIconContentLayout
    });

   myMap.geoObjects
       .add(myPlacemarkWithContent)
       .add(myPlacemarkWithContent1);
});
function onclick1() {
     ymaps.geocode('улица Малиновского, 59 Макеевка', {      
        results: 1
    }).then(function (res) {
            // Выбираем первый результат геокодирования.
            var firstGeoObject = res.geoObjects.get(0),
                // Координаты геообъекта.
                coords = firstGeoObject.geometry.getCoordinates(),
                // Область видимости геообъекта.
                bounds = firstGeoObject.properties.get('boundedBy');

            console.log(bounds);
            // Масштабируем карту на область видимости геообъекта.
            myMap.setBounds(bounds, {
                // Проверяем наличие тайлов на данном масштабе.
                checkZoomRange: true
            });

           
        });
}
function onclick2() {
    ymaps.geocode('Украина, Донецкая область, Макеевка, Центрально-Городской район, Московская улица, 9', {      
       results: 1
   }).then(function (res) {
           // Выбираем первый результат геокодирования.
           var firstGeoObject = res.geoObjects.get(0),
               // Координаты геообъекта.
               coords = firstGeoObject.geometry.getCoordinates(),
               // Область видимости геообъекта.
               bounds = firstGeoObject.properties.get('boundedBy');

               console.log(bounds);
           // Масштабируем карту на область видимости геообъекта.
           myMap.setBounds(bounds, {
               // Проверяем наличие тайлов на данном масштабе.
               checkZoomRange: true
           });

          
       });
}

