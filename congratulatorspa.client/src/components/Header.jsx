import React from "react";
import { Link } from 'react-router-dom'

const Header = () => {
    return (
        <header>
            <nav className="nav-bar">
                <div className="image">
                    <img src="/congratulator.png" alt="icon" />
                </div>
                <div className="nav-left">
                    <Link to="/" className="brand">Home</Link>
                </div>
                <div className="nav-right">
                    <ul className="nav-list">
                        <li><Link to="/allbirthdays">All birthdays</Link></li>
                    </ul>
                </div>
            </nav>        
        </header>
    )
}

export default Header