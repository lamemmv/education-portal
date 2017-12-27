import { h, Component } from "preact";
import { Link, HashRouter } from 'react-router-dom';
import { Text } from 'preact-i18n';

class EPHeader extends Component {
    render() {
        return (
            < HashRouter >
                <header class="mdc-toolbar mdc-toolbar--fixed">
                    <div class="mdc-toolbar__row">
                        <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
                            <Link to="/" class="mdc-toolbar__menu-icon"><Text id='header.home'></Text></Link>
                            {/* <span class="mdc-toolbar__title"><Text id='header.menu'></Text></span> */}
                        </section>
                        <section class="mdc-toolbar__section">
                            <Link to="/files" class="mdc-toolbar__menu-icon"><Text id='header.files'></Text></Link>
                        </section>
                        <section class="mdc-toolbar__section mdc-toolbar__section--align-end mdc-toolbar__section--shrink-to-fit">
                            <Link to="/news" class="mdc-toolbar__menu-icon"><Text id='header.news'></Text></Link>
                        </section>
                    </div>
                </header>
            </HashRouter>
        );
    };
}

export default EPHeader;
