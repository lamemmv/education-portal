import { h, Component } from "preact";
import { Link, HashRouter } from 'react-router-dom';
import { Text } from 'preact-i18n';
import logo from '../../assets/images/logo.png';
import linkedin from '../../assets/images/in-128-2.png';
import twitter from '../../assets/images/twitter-128.png';
import facebook from '../../assets/images/facebook-128.png';
import googleplus from '../../assets/images/google_plus-128.png';

class EPHeader extends Component {
    render() {
        return (
            < HashRouter >
                <header class="mdc-toolbar mdc-toolbar--fixed mdc-toolbar--fixed-lastrow-only ep-toolbar">
                    <div class="mdc-toolbar__row ep-toolbar__row">
                        <div class='ep-toolbar__section'>
                            <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
                                <div class='ep-toolbar_item'>
                                    <i class="material-icons">phone</i>
                                    <span>(+84)986-448-474</span>
                                </div>
                                <div class='ep-toolbar_item'>
                                    <i class="material-icons">email</i>
                                    <span>contact@education-portal.com</span>
                                </div>
                            </section>
                            <section class="mdc-toolbar__section ep-toolbar__section__end">
                                <div class="social-icon">
                                    <a href="#" target="_blank">
                                        <img width="32" height="32" src={facebook} alt="Facebook" />
                                    </a>
                                </div>
                                <div class="social-icon">
                                    <a href="#" target="_blank">
                                        <img width="32" height="32" src={linkedin} alt="Linkedin" />
                                    </a>
                                </div>
                                <div class="social-icon">
                                    <a href="#" target="_blank">
                                        <img width="32" height="32" src={twitter} alt="Twitter" />
                                    </a>
                                </div>
                                <div class="social-icon">
                                    <a href="#" target="_blank">
                                        <img width="32" height="32" src={googleplus} alt="Twitter" />
                                    </a>
                                </div>
                                <div class='ep-toolbar__end'>
                                    <i class="material-icons">lock</i>
                                    <Link to='/login'>
                                        <span><Text id='signin'></Text></span>
                                    </Link>
                                    <span class="ep-separator">|</span>
                                    <Link to='/signup'>
                                        <span><Text id='signup'></Text></span>
                                    </Link>
                                </div>
                            </section>
                        </div>
                    </div>
                    <div class="mdc-toolbar__row ep-toolbar__row">
                        <div class='ep-toolbar__section'>
                            <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
                                <Link to='/' class='ep-logo'>
                                    <span><Text id='home'></Text></span>
                                </Link>
                            </section>
                            <section class="mdc-toolbar__section">

                            </section>
                            <section class="mdc-toolbar__section mdc-toolbar__section--align-end">
                                <Link to="/" class="mdc-toolbar__menu-icon ep-menu-icon ep-menu-icon">
                                    <Text id='header.home'></Text>
                                </Link>
                                <Link to="/files" class="mdc-toolbar__menu-icon ep-menu-icon ep-menu-icon">
                                    <Text id='header.files'></Text>
                                </Link>
                                <Link to="/news" class="mdc-toolbar__menu-icon ep-menu-icon ep-menu-icon">
                                    <Text id='header.news'></Text>
                                </Link>
                                <Link to="/courses" class="mdc-toolbar__menu-icon ep-menu-icon ep-menu-icon">
                                    <Text id='header.courses'></Text>
                                </Link>
                                <Link to="/contact" class="mdc-toolbar__menu-icon ep-menu-icon ep-menu-icon">
                                    <Text id='header.contact'></Text>
                                </Link>
                            </section>
                        </div>
                    </div>
                </header>
            </HashRouter>
        );
    };
}

export default EPHeader;
