jQuery(document).ready(function($){
    new WOW().init();

    $('.tab-wrapper').each(function() {
        let $tabWrapper, $tabID;
		$tabWrapper = $(this);
		$tabID = $tabWrapper.find('.tab-link.current').attr('data-tab');
        $tabWrapper.find($tabID).fadeIn().siblings().hide();
        $($tabWrapper).on('click', '.tab-link', function(e){
            e.preventDefault();
			$tabID = $(this).attr('data-tab');
			$(this).addClass('current').siblings().removeClass('current');
			$tabWrapper.find($tabID).fadeIn().siblings().hide();
        });
    });

    $('html').on('click', '.main-menu-btn', function(){
        $(this).addClass('active');
        $('.main-menu').addClass('active');
    });

    $('html').on('click', '.main-menu-overlay', function(){
        $('.main-menu-btn').removeClass('active');
        $('.main-menu').removeClass('active');
    });

    $(".acc-info-btn").on('click', function(e){
        e.preventDefault();
		$(".status-mobile").addClass("open");
		$(".overlay-status-mobile").show();
    });
    
	$(".overlay-status-mobile").on('click', function(){
		$(".status-mobile").removeClass("open");
		$(this).hide();
    });

    if($("#brand-source").length){
        $("#brand-source").msDropdown();
    };

    if ($('.scroll-top').length) {
		$(window).scroll(function() {
			$(this).scrollTop() > 100 ? $('.scroll-top').addClass('show') : $('.scroll-top').removeClass('show');
		});
		$('html').on('click', '.scroll-top', function(){
			$('html, body').animate({ scrollTop: 0 }, 'slow');
		})
    };

    if (window.matchMedia("(max-width: 768px)").matches) {};

    if($('.header').length && $('.main').length){
        let $header = $('.header'), $main = $('.main');
        $main.css('margin-top', $header.outerHeight());
        $(window).scrollTop() > 0 ? $header.addClass('fixed') : $header.removeClass('fixed');
        $(window).on('scroll', function(){
            $(window).scrollTop() > 0 ? $header.addClass('fixed') : $header.removeClass('fixed');
        })
    };
});