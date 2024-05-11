"use client";

const Pied = () => {

    return(
        <footer className="bg-gray-800 text-white pt-8 pb-2 flex flex-col items-center justify-center">
    <div className="flex items-center mb-4">
        <div className="flex flex-col items-center mr-5">
            <img src="/imgs/logo.png" alt="Logo du jeu" className="h-20 mb-2" />
        </div>
        <div className="flex flex-col items-center ml-20">
            <h2 className="text-xl font-semibold">Nous rejoindre</h2>
            
            <a href="https://www.instagram.com/okey_bytes/" target="_blank" rel="noopener noreferrer" className="flex">
                <p>Sur Instagram</p>
                <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8 fill-current" viewBox="0 0 24 24">
                    <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm4.1 3.5c.3 0 .5.2.5.5v9c0 .3-.2.5-.5.5h-8.2c-.3 0-.5-.2-.5-.5v-9c0-.3.2-.5.5-.5h8.2zm-1.3 2.5c-.5 0-1 .4-1 1v6c0 .6.4 1 1 1h-6c-.6 0-1-.4-1-1v-6c0-.6.4-1 1-1h6zm-3.1 2.3c-.8 0-1.5.7-1.5 1.5s.7 1.5 1.5 1.5 1.5-.7 1.5-1.5-.7-1.5-1.5-1.5zM12 14c1.5 0 2.7-1.2 2.7-2.7S13.5 8.7 12 8.7 9.3 9.9 9.3 11.5 10.5 14 12 14z"/>
                </svg>
            </a>
        </div>
    </div>
    <div className="flex items-center justify-center w-full">
        <p>OkeyBytes Studio tous droits réservés</p>
    </div>
</footer>


    
    );

};


export default Pied;
