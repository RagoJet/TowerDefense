mergeInto(LibraryManager.library, {

    SaveJSONToYAExtern: function (date) {
        var dateString = UTF8ToString(date);
        var myObj = JSON.parse(dateString);
        // player.setData(myObj);
        window.player.setData(myObj);
    },

    LoadJSONFromYAExtern: function () {
        window.player.getData().then(_date => {
            const MyJSON = JSON.stringify(_date);
            myGameInstance.SendMessage('GameManager', 'Init', MyJSON);
        })
    },

    RateGameExtern: function () {
        window.ysdk.feedback.canReview()
            .then(({value, reason}) => {
                if (value) {
                    window.ysdk.feedback.requestReview()
                        .then(({feedbackSent}) => {
                            console.log(feedbackSent);
                        })
                } else {
                    console.log(reason)
                }
            })
    },

    GetLangExtern: function () {
        var lang = window.ysdk.environment.i18n.lang;
        var bufferSize = lengthBytesUTF8(lang) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(lang, buffer, bufferSize);
        return buffer;
    },

    ShowAdvExtern: function () {
        window.ysdk.adv.showFullscreenAdv({
            callbacks: {
                onOpen: function () {
                    myGameInstance.SendMessage('GameManager', 'Pause');
                },
                onClose: function (wasShown) {
                    myGameInstance.SendMessage('GameManager', 'UnPause');
                },
                onError: function (error) {
                    myGameInstance.SendMessage('GameManager', 'UnPause');
                }
            }
        })

    },

    WatchAdsExtern: function () {
        window.ysdk.adv.showRewardedVideo({
            callbacks: {
                onOpen: () => {
                    myGameInstance.SendMessage('GameManager', 'Pause');
                    console.log('Video ad open.');
                },
                onRewarded: () => {
                    console.log('Rewarded!');
                    myGameInstance.SendMessage('Shop', 'AddGoldFromAd');
                },
                onClose: () => {
                    myGameInstance.SendMessage('GameManager', 'UnPause');
                    console.log('Video ad closed.');
                },
                onError: (e) => {
                    myGameInstance.SendMessage('GameManager', 'UnPause');
                    console.log('Error while open video ad:', e);
                }
            }
        })

    },

});