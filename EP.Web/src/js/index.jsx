import {
    h,
    render
} from 'preact';
import { IntlProvider } from 'preact-i18n';

import styles from 'react-responsive-carousel/lib/styles/carousel.min.css';

import 'toastr/build/toastr.min.css';
import 'bootstrap';
import 'bootstrap-material-design/dist/css/bootstrap-material-design.min.css';
import 'bootstrap-material-design/dist/js/bootstrap-material-design.min';

import definition from './locales/vi.json';
import App from './components/App.jsx';
import configureStore from './store/configureStore';

const store = configureStore();

if (process.env.NODE_ENV !== 'production') {
    console.log('Looks like we are in development mode!');
}

const renderApp = (rootId = 'root') => {
    const targetEl = document.getElementById(rootId);

    render((<IntlProvider definition={definition}>
        <App store={store} />
    </IntlProvider>), targetEl);
}

renderApp();

(function () {
    $(document).ready(function () {

        var toggleAffix = function (affixElement, scrollElement, wrapper) {

            var height = affixElement.outerHeight(),
                top = wrapper.offset().top;

            if (scrollElement.scrollTop() >= top) {
                wrapper.height(height);
                affixElement.addClass("affix");
            }
            else {
                affixElement.removeClass("affix");
                wrapper.height('auto');
            }

        };


        $('[data-toggle="affix"]').each(function () {
            var ele = $(this),
                wrapper = $('<div></div>');

            ele.before(wrapper);
            $(window).on('scroll resize', function () {
                toggleAffix(ele, $(this), wrapper);
            });

            // init
            toggleAffix(ele, $(window), wrapper);
        });

    });
})();