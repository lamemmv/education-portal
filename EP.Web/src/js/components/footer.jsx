import { h, Component } from "preact";
import { Link, HashRouter } from 'react-router-dom';
import { Text } from 'preact-i18n';

class EPFooter extends Component {
    render() {
        return (
            < HashRouter >
                <footer class="page-footer example">
                    <div class="container">
                        <div class="row">
                            <div class="col l6 s12">
                                <h5 class="white-text">Cơ sở dạy thêm TRÍ NHÂN</h5>
                                <p class="grey-text text-lighten-4">249/9 Đường Vườn Lài, Phường Phú Thọ Hòa, Quận Tân Phú, TP.HCM</p>
                            </div>
                            <div class="col l4 offset-l2 s12">
                                <h5 class="white-text">Links</h5>
                                <ul>
                                    <li><a class="grey-text text-lighten-3" href="#!">Link 1</a></li>
                                    <li><a class="grey-text text-lighten-3" href="#!">Link 2</a></li>
                                    <li><a class="grey-text text-lighten-3" href="#!">Link 3</a></li>
                                    <li><a class="grey-text text-lighten-3" href="#!">Link 4</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="footer-copyright">
                        <div class="container">
                            © 2014 Copyright Education Portal team.
                            <a class="grey-text text-lighten-4 right" href="#!">More Links</a>
                        </div>
                    </div>
                </footer>
            </HashRouter>
        );
    };
}

export default EPFooter;
