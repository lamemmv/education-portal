import { h, Component } from "preact";
import { Link, HashRouter } from 'react-router-dom';
import { Text } from 'preact-i18n';
import logo from '../../assets/images/logo.png';

class EPHeader extends Component {
    render() {
        return (
            < HashRouter >
                <header class="mdc-toolbar mdc-toolbar--fixed mdc-toolbar--fixed-lastrow-only ep-toolbar">
                    <div class="mdc-toolbar__row ep-toolbar__row">
                        <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
                            <div style="margin: 0px 10px; display: inline-block; *display: inline; *zoom:1;">
                                <i class="gdlr-icon icon-phone fa fa-phone" style="color: #bababa; font-size: 14px; "></i>(+84)986-448-474
                            </div>
                            <div style="margin: 0px 10px ; display: inline-block; *display: inline;  *zoom:1;">
                                <i class="gdlr-icon icon-envelope fa fa-envelope" style="color: #bababa; font-size: 14px; "></i>contact@education-portal.com
                            </div>
                        </section>
                        <section class="mdc-toolbar__section">
                            <div class="social-icon">
                                <a href="#" target="_blank">
                                    <img width="16" height="16" src="http://demo.goodlayers.com/clevercourse/wp-content/themes/clevercourse/images/dark/social-icon/facebook.png" alt="Facebook" />
                                </a>
                            </div>
                            <div class="social-icon">
                                <a href="#" target="_blank">
                                    <img width="16" height="16" src="http://demo.goodlayers.com/clevercourse/wp-content/themes/clevercourse/images/dark/social-icon/linkedin.png" alt="Linkedin" />
                                </a>
                            </div>
                            <div class="social-icon">
                                <a href="#" target="_blank">
                                    <img width="16" height="16" src="http://demo.goodlayers.com/clevercourse/wp-content/themes/clevercourse/images/dark/social-icon/twitter.png" alt="Twitter" />
                                </a>
                            </div>
                        </section>
                    </div>
                    <div class="mdc-toolbar__row ep-toolbar__row">
                        <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
                            Section aligns to start.
                        </section>
                        <section class="mdc-toolbar__section">
                            Section aligns to center.
                        </section>
                        <section class="mdc-toolbar__section mdc-toolbar__section--align-end">
                            <Link to="/" class="mdc-toolbar__menu-icon ep-menu-icon ep-menu-icon"><Text id='header.home'></Text></Link>
                            <Link to="/files" class="mdc-toolbar__menu-icon ep-menu-icon ep-menu-icon"><Text id='header.files'></Text></Link>
                            <Link to="/news" class="mdc-toolbar__menu-icon ep-menu-icon ep-menu-icon"><Text id='header.news'></Text></Link>
                        </section>
                    </div>
                    {/* <div class="top-navigation-wrapper">
                        <div class='top-navigation-container container'>
                            <div class="top-navigation-left">
                                <div class="top-navigation-left-text">
                                    <div style="margin: 0px 10px; display: inline-block; *display: inline; *zoom:1;">
                                        <i class="gdlr-icon icon-phone fa fa-phone" style="color: #bababa; font-size: 14px; "></i>(+84)986-448-474
                                    </div>
                                    <div style="margin: 0px 10px ; display: inline-block; *display: inline;  *zoom:1;">
                                        <i class="gdlr-icon icon-envelope fa fa-envelope" style="color: #bababa; font-size: 14px; "></i>contact@education-portal.com
                                    </div>
                                </div>
                            </div>
                            <div class='top-navigation-right'>
                                <div class='top-social-wrapper'>
                                    <div class="social-icon">
                                        <a href="#" target="_blank">
                                            <img width="16" height="16" src="http://demo.goodlayers.com/clevercourse/wp-content/themes/clevercourse/images/dark/social-icon/facebook.png" alt="Facebook" />
                                        </a>
                                    </div>
                                    <div class="social-icon">
                                        <a href="#" target="_blank">
                                            <img width="16" height="16" src="http://demo.goodlayers.com/clevercourse/wp-content/themes/clevercourse/images/dark/social-icon/linkedin.png" alt="Linkedin" />
                                        </a>
                                    </div>
                                    <div class="social-icon">
                                        <a href="#" target="_blank">
                                            <img width="16" height="16" src="http://demo.goodlayers.com/clevercourse/wp-content/themes/clevercourse/images/dark/social-icon/twitter.png" alt="Twitter" />
                                        </a>
                                    </div>
                                </div>
                                <div class="gdlr-lms-header-signin">
                                    <i class="fa fa-lock icon-lock"></i>
                                    <a data-rel="gdlr-lms-lightbox" data-lb-open="login-form">Sign In</a>
                                    <span class="gdlr-separator">|</span>
                                    <a href="http://demo.goodlayers.com/clevercourse?register=http://demo.goodlayers.com/clevercourse/">Sign Up</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="gdlr-header-inner">
                        <div class='gdlr-header-container container'>
                            <div class='gdlr-logo'>
                                <Link to='/' style={{display: 'grid'}}>
                                    <img height='102' width='102' src={logo} alt="" />
                                    <span><Text id='home'></Text></span>
                                </Link>
                            </div>
                            <div class='gdlr-navigation-wrapper'>
                                <nav class='gdlr-navigation'>
                                    <ul class='sf-menu gdlr-main-menu'>
                                        <li class="menu-item menu-item-type-post_type menu-item-object-page menu-item-home menu-item-3585menu-item menu-item-type-post_type menu-item-object-page menu-item-home menu-item-3585 gdlr-normal-menu">
                                            <Link to="/" class="mdc-toolbar__menu-icon ep-menu-icon"><Text id='header.home'></Text></Link>
                                        </li>
                                        <li class="menu-item menu-item-type-post_type menu-item-object-page menu-item-has-children menu-item-3600menu-item menu-item-type-post_type menu-item-object-page menu-item-has-children menu-item-3600 gdlr-normal-menu">
                                            <Link to="/files" class="mdc-toolbar__menu-icon ep-menu-icon"><Text id='header.files'></Text></Link>
                                        </li>
                                        <li class="menu-item menu-item-type-post_type menu-item-object-page menu-item-has-children menu-item-3600menu-item menu-item-type-post_type menu-item-object-page menu-item-has-children menu-item-3600 gdlr-normal-menu">
                                            <Link to="/news" class="mdc-toolbar__menu-icon ep-menu-icon"><Text id='header.news'></Text></Link>
                                        </li>
                                    </ul>
                                </nav>
                            </div>
                        </div>
                    </div> */}
                </header>
            </HashRouter>
        );
    };
}

export default EPHeader;
