import { h, Component } from "preact";
import { Link, HashRouter } from 'react-router-dom';
import { Localizer, Text } from 'preact-i18n';
import logo from '../../assets/images/logo.png';
import linkedin from '../../assets/images/in-128-2.png';
import twitter from '../../assets/images/twitter-128.png';
import facebook from '../../assets/images/facebook-128.png';
import googleplus from '../../assets/images/google_plus-128.png';

class EPHeader extends Component {
    render() {
        return (
            < HashRouter >
                <div class='container'>
                    <nav class="navbar navbar-expand navbar-light ep-navbar">
                        <button class="navbar-toggler"
                            type="button"
                            data-toggle="collapse"
                            data-target="#topbarInformation"
                            aria-controls="topbarInformation"
                            aria-expanded="false"
                            aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>

                        <div class="collapse navbar-collapse" id="topbarInformation">
                            <ul class="navbar-nav mr-auto">
                                <li class="nav-item nav-link ep-nav-item">
                                    <i class="material-icons">phone</i>
                                    <span>(+84)909 972 559</span>
                                </li>
                                <li class="nav-item nav-link ep-nav-item">
                                    <i class="material-icons">email</i>
                                    <span>trinhanmail@gmail.com</span>
                                </li>
                            </ul>
                            <ul class="navbar-nav ml-auto">
                                <li class="nav-item">
                                    <a class="nav-link"><img width="32" height="32" src={facebook} alt="Facebook" /></a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link"><img width="32" height="32" src={linkedin} alt="Linkedin" /></a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link"><img width="32" height="32" src={twitter} alt="Twitter" /></a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link"><img width="32" height="32" src={googleplus} alt="GooglePlus" /></a>
                                </li>
                                <li class="nav-item ep-vertical-divider">
                                </li>
                                <li class="nav-item">
                                    <Link to='/login' class='nav-link ep-nav-item-x'>
                                        <i class="material-icons">lock</i>
                                        <span><Text id='signin'></Text></span>
                                    </Link>
                                </li>
                                <li class="navbar-item">
                                    <Link to='/signup' class='nav-link ep-nav-item-x'>
                                        <span><Text id='signup'></Text></span>
                                    </Link>
                                </li>

                            </ul>
                        </div>
                    </nav>
                    <nav class="navbar navbar-expand-md navbar-light ep-navbar" data-toggle="affix">
                        <Link to="/" class="navbar-brand ep-navbar-brand">
                            <Text id='home'></Text>
                        </Link>
                        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>
                        <div class="collapse navbar-collapse" id="navbarCollapse">
                            <ul class="navbar-nav ml-auto">
                                <li class="nav-item active">
                                    <Link to="/" class='nav-link ep-nav-link'>
                                        <Text id='header.about'></Text>
                                    </Link>
                                </li>
                                <li class="nav-item">
                                    <Link to="/" class='nav-link ep-nav-link'>
                                        <Text id='header.parents'></Text>
                                    </Link>
                                </li>
                                <li class="nav-item">
                                    <Link to="/" class='nav-link ep-nav-link'>
                                        <Text id='header.examination'></Text>
                                    </Link>
                                </li>
                                <li class="nav-item">
                                    <Link to="/files" class='nav-link ep-nav-link'>
                                        <Text id='header.files'></Text>
                                    </Link>
                                </li>
                                <li class="nav-item">
                                    <Link to="/news" class='nav-link ep-nav-link'>
                                        <Text id='header.news'></Text>
                                    </Link>
                                </li>
                                <li class="nav-item">
                                    <Link to="/courses" class='nav-link ep-nav-link'>
                                        <Text id='header.recruitments'></Text>
                                    </Link>
                                </li>
                                <li class="nav-item">
                                    <Link to="/contact" class='nav-link ep-nav-link'>
                                        <Text id='header.contact'></Text>
                                    </Link>
                                </li>
                            </ul>
                        </div>
                    </nav>
                </div>
            </HashRouter>
        );
    };
}

export default EPHeader;
