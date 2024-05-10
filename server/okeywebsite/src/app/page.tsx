import OSInfo from './OSButton';
import Hero from './Hero';
import NavBar from './Navbar';
import OkeyHistory from "./History";
import Tutoriel from './Tutoriel';

export default function Home() {
  return (
    <div>
        <NavBar />
        <Hero />
        <OkeyHistory />
        <Tutoriel />
    </div>
  );
}
