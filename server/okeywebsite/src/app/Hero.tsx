import React from 'react';
import OSInfo from './OSButton';
import Image from 'next/image';

const Hero = () => {
  return (
    <div className='text-white bg-cover bg-center h-screen' style={{backgroundImage: "url('./imgs/bg-green.png')"}}>
        <div className='max-w-[800px] mx-auto h-full flex flex-col justify-center items-center'>

            <div className="mb-8 flex justify-center">
                <Image src="/imgs/logo.png" alt="OkeyGame Logo" width={270} height={270} />
            </div>

            <p className='md:text-2xl text-xl font-bold text-white mb-4'>Le jeu de tuiles passionnant</p>
            <div className="space-x-4 mb-4">
                <OSInfo />
                <button className="bg-primaryColor hover:bg-primaryColorHover text-black py-2 px-4 rounded">
                    Essayer sur Android
                </button>
            </div>
        </div>
    </div>
  );
};

export default Hero;