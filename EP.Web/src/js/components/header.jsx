import { h, Component } from "preact";
import { Link, HashRouter } from 'react-router-dom';

class EPHeader extends Component {
    render() {
        return (
            <HashRouter>
                <header class="mdc-toolbar mdc-toolbar--fixed">
                    <div class="mdc-toolbar__row">
                        <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
                            <Link to="/" class="mdc-toolbar__menu-icon">menu</Link>
                            <span class="mdc-toolbar__title">Title</span>
                        </section>
                        <section class="mdc-toolbar__section">
                            <Link to="/files" class="mdc-toolbar__menu-icon">files</Link>
                        </section>
                        <section class="mdc-toolbar__section mdc-toolbar__section--align-end mdc-toolbar__section--shrink-to-fit">
                            <Link to="/news" class="mdc-toolbar__menu-icon">news</Link>
                        </section>
                    </div>
                </header>
            </HashRouter>
        );
    };
}

export default EPHeader;
