import React from 'react';
import OSInfo from './OSButton';

const Hero = () => {

  
  return (

    <div className='text-white bg-cover bg-center h-screen'>
  <div className='bg-cover h-screen bg-center flex justify-center items-center' style={{backgroundImage: "url('./imgs/bg-green.png')"}}>
    <div className='max-w-[800px] mx-4 flex-1'>
      <div className="md:flex md:justify-between">
        <div className="flex-1 mr-2">
          <h1 className='md:text-4xl text-4xl font-bold'>Le jeu qui réunis et enchante</h1> <br />
          <p className='font-normal text-white mb-4'>Faites preuve de stratégie, jouez en ligne avec vos amis, votre famille ou avec notre communauté de joueurs.</p>
          <div className="space-x-4">
            <OSInfo />
            <button className="bg-primaryColor hover:bg-primaryColorHover text-black py-2 px-4 rounded">
              Essayer sur Android
            </button>
          </div>
        </div>

        <div className="flex-1 ml-2">
          <video className='h-80' controls>
            Your browser does not support the video tag.
          </video>
        </div>
      </div>
    </div>
  </div>      
</div>


  );
};

export default Hero;
