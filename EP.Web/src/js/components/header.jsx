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
                <nav class="navbar navbar-expand-md navbar-dark sticky-top">
                    <Link to="/" class="navbar-brand ep-navbar-brand">
                        <Text id='header.home'></Text>
                    </Link>
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarCollapse">
                        <ul class="navbar-nav mr-auto">
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
                        <form class="form-inline mt-2 mt-md-0">
                            <Localizer>
                                <input class="form-control mr-sm-2" type="text" placeholder={<Text id='search'></Text>} aria-label="Search" />
                            </Localizer>
                            <button class="btn btn-outline-success my-2 my-sm-0" type="submit"><Text id='search'></Text></button>
                        </form>
                    </div>
                </nav>
            </HashRouter>
        );
    };
}

export default EPHeader;
