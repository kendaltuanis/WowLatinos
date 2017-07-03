        $(document).ready(function () {
            "use strict";

            var revapi;
            if ($("#rev_slider").revolution == undefined) {
                revslider_showDoubleJqueryError("#rev_slider");
            } else {
                revapi = $("#rev_slider").show().revolution({
                    sliderType: "standard",
                    jsFileLocation: "js/",
                    sliderLayout: "fullscreen",
                    fullScreenOffsetContainer: ".header,.fixed-footer",
                    dottedOverlay: "none",
                    delay: 10000,
                    navigation: {
                        keyboardNavigation: "off",
                        keyboard_direction: "horizontal",
                        mouseScrollNavigation: "off",
                        onHoverStop: "off",
                        touch: {
                            touchenabled: "on",
                            swipe_threshold: 75,
                            swipe_min_touches: 1,
                            swipe_direction: "horizontal",
                            drag_block_vertical: false
                        },
                        arrows: {
                            style: "zeus",
                            enable: true,
                            hide_onmobile: true,
                            hide_under: 600,
                            hide_onleave: true,
                            hide_delay: 200,
                            hide_delay_mobile: 1200,
                            tmp: '<div class="tp-title-wrap"><div class="tp-arr-imgholder"></div></div>',
                            left: {
                                h_align: "left",
                                v_align: "center",
                                h_offset: 86,
                                v_offset: 0
                            },
                            right: {
                                h_align: "right",
                                v_align: "center",
                                h_offset: 86,
                                v_offset: 0
                            }
                        },
                        bullets: {
                            enable: true,
                            hide_onmobile: true,
                            hide_under: 600,
                            style: "metis",
                            hide_onleave: true,
                            hide_delay: 200,
                            hide_delay_mobile: 1200,
                            direction: "horizontal",
                            h_align: "center",
                            v_align: "bottom",
                            h_offset: 0,
                            v_offset: 22,
                            space: 6,
                            tmp: '<span class="tp-bullet-img-wrap"><span class="tp-bullet-image"></span></span>'
                        }
                    },
                    viewPort: {
                        enable: true,
                        outof: "pause",
                        visible_area: "80%"
                    },
                    responsiveLevels: [1240, 1024, 778, 480],
                    gridwidth: [1240, 1024, 778, 480],
                    gridheight: [600, 600, 500, 400],
                    lazyType: "none",
                    parallax: {
                        type: "mouse",
                        origo: "slidercenter",
                        speed: 2000,
                        levels: [2, 3, 4, 5, 6, 7, 12, 16, 10, 50],
                    },
                    shadow: 0,
                    spinner: "off",
                    stopLoop: "off",
                    stopAfterLoops: -1,
                    stopAtSlide: -1,
                    shuffle: "off",
                    autoHeight: "off",
                    hideThumbsOnMobile: "off",
                    hideSliderAtLimit: 0,
                    hideCaptionAtLimit: 0,
                    hideAllCaptionAtLilmit: 0,
                    debugMode: false,
                    fallbacks: {
                        simplifyAll: "off",
                        nextSlideOnWindowFocus: "off",
                        disableFocusListener: false,
                    }
                });


                $(window).on('resize revolution.slide.onloaded', function () {
                    var o = $("#rev_slider").data('opt');
                    if ($(this).width() < 768) {
                        o.fullScreenOffsetContainer = '.header';
                        o.arrows = {
                            left: { h_offset: 25 },
                            right: { h_offset: 25 }
                        };
                    } else {
                        o.fullScreenOffsetContainer = '.header, .fixed-footer';
                    }
                    $("#rev_slider").data('opt', o).revredraw();
                });
            }

        });