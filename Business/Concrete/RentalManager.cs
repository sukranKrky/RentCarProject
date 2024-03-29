﻿using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        ICarDal _carDal;
        ICustomerDal _customerDal;


        public RentalManager(IRentalDal rentalDal, ICarDal carDal,ICustomerDal customerDal)
        {
            _rentalDal = rentalDal;
            _carDal = carDal;
            _customerDal = customerDal;
            
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {
            var result = _rentalDal.GetAll(r => r.CarId == rental.CarId);
            

            foreach (var res in result)
            {
                if (res.ReturnDate==null|| rental.ReturnDate <= res.ReturnDate)
                {
                    
                    return new ErrorResult(Messages.RentalNotAdded);
                }
                


            }
            _rentalDal.Add(rental);
            return new SuccessResult(Messages.RentalAdded);

        }


        
        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult();
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll());
            
        }

        public IDataResult<Rental> GetById(int id)
        {
            return new SuccessDataResult<Rental>(_rentalDal.Get(r=>r.RentalId==id) );
        }

        public IDataResult<List<RentalDateilDto>> GetRentalDetails(int carId)
        {
            return new SuccessDataResult<List<RentalDateilDto>>(_rentalDal.GetRentalDetails(c=>c.CarId==carId));
        }

        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult();
        }
    }
}
